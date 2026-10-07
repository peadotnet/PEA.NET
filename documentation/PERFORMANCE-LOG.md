# Performance log

End-to-end performance history of PEA.NET, measured on a fixed reference workload.

## Reference workload

`Examples/PEA_TSP_Example` — Berlin52, seed `20250421`, 1 island, steady-state.

The seed is fixed, so **the work is identical between runs**: every run evaluates
exactly 9,006,240 entities and reaches a best distance of 7849.395176892104. Only
wall-clock time varies, by roughly ±1%. A change below ~2% is therefore noise; above
that it is signal.

Measure with:

```
dotnet build src/PEA/Examples/PEA_TSP_Example/PEA_TSP_Example.csproj -c Release
cd src/PEA/Examples/PEA_TSP_Example && dotnet run -c Release --no-build
```

Run three times, take the mean of `Elapsed`. Record both the micro-benchmark result
(what the isolated operation costs) and the macro result (what the whole run costs),
because they do not always agree — see 2026-09-29 below.

## History

| Date | Change | Micro-benchmark | Share of total | Macro before | Macro after | End-to-end gain |
|---|---|---|---|---:|---:|---:|
| 2026-09-19 | Index maintenance moved from indexer getter to mutation (`FlatPopulation`, `EntityList`) | indexer read 0.61–0.86× | not measured | — | — | not measured |
| 2026-09-29 | `EntityMutation` mutates offspring in place; no `Clone(true)` per offspring | alloc 0.20–0.27×, time 0.28–0.59× | mutation ≈ 29% of the old run | 17 693 ms | 13 748 ms | **−22.3%** (1.29× faster) |
| 2026-09-30 | `try/catch (Exception)` removed from `EntityCrossover.Cross` | — | crossover ≈ 4.8% | 13 748 ms | 13 120 ms | **−4.6%** (cumulative −25.8%) |
| 2026-10-07 | `EntityCrossover` throws if a crossover returns a parent instance (`ReferenceEquals` guard) | — | — | 13 441 ms (A) | 13 536 ms (B) | +0.7%, **within noise** |
| 2026-10-07 | `EntityCrossover` iterates a cached `string[]` of chromosome names instead of `parents[0].Chromosomes.Keys` | — | — | 14 174 ms (A) | 14 283 ms (B) | +0.8%, **within noise** |

### 2026-09-29 — in-place mutation

Macro runs (ms):

- before: 17 545 / 18 300 / 17 233 → mean **17 693**
- after: 13 691 / 13 852 / 13 701 → mean **13 748**

Derived shares, using the micro-benchmark cost of ~130 ns per mutated entity and
9,006,240 entities per run:

- mutation after the change: ~1 167 ms = **8.5%** of the new total
- mutation before the change: ~5 112 ms = **28.9%** of the old total

**The macro gain exceeded what the micro-benchmark predicted.** The micro-benchmark
time ratio was 0.28–0.59, which on a 29% share predicts roughly a 12–21% end-to-end
gain; the measured gain was 22.3%. The gap is the deferred cost the micro-benchmark
does not attribute: the removed clones were surviving gen0 and being **promoted to
gen1** (Gen1 column 2.93 → 0.18 at 100 offspring), and that collection cost is paid
later, outside the measured operation.

Take-away for future entries: a micro-benchmark under-states the benefit of removing
allocations that get promoted. Always record the macro number too.

## Step profile (2026-09-30, measured after the in-place mutation change)

Collected with `dotnet-trace` (`dotnet-sampled-thread-time`), converted to speedscope,
inclusive time per frame. `SteadyStateAlgorithm.RunOnce` = 15 100 ms = 100%.

| Step | ms | Share of the loop |
|---|---:|---:|
| **Evaluate** (`IslandLocalRunner.Evaluate` -> `TSPEvaluation.Decode`) | 11 126 | **73.7%** |
| **Selection** (`TournamentSelection.SelectWithList`) | 2 069 | **13.7%** |
| **Mutation** (`EntityMutation.Mutate`) | 1 143 | **7.6%** |
| **Crossover** (`EntityCrossover.Cross`) | 730 | **4.8%** |
| MergeToBests | 22 | 0.1% |
| StopCriteria | 17 | 0.1% |
| Reinsertion (`ReplaceParentsOnlyWithBetter`) | 7 | 0.0% |

The steps sum to ~100%, so nothing significant is unaccounted for.

Two things this changes:

1. **Evaluation dominates at 73.7%** — but `TSPEvaluation.Decode` is *example* code, not
   library code. For the library itself the addressable budget is the remaining ~26%.
   Any user whose evaluation is expensive will see the same shape, which is an argument
   for documenting this: the framework's overhead is small relative to a real fitness
   function.
2. **Selection (13.7%) costs almost twice as much as mutation (7.6%), and nearly three
   times crossover.** It is the largest library-side cost and has not been optimized.

Collect a new profile with:

```
dotnet tool install --global dotnet-trace --add-source https://api.nuget.org/v3/index.json
cd src/PEA/Examples/PEA_TSP_Example/bin/Release/net8.0
dotnet-trace collect --output tsp.nettrace --profile dotnet-sampled-thread-time   --duration 00:00:00:25 -- "$PWD/PEA_TSP_Example.exe"
dotnet-trace convert --format speedscope tsp.nettrace
```

Note the pipeline wrappers in `GeneticAlgorithmBase` (`Evaluate`, `Crossover`, `Mutate`,
...) are inlined in Release and do **not** appear as stack frames. Match on the operator
classes instead.

## Method for estimating a step's share

1. Measure the operation's per-entity cost with a micro-benchmark (`PEA.Benchmarks`).
2. Multiply by 9,006,240 (entities per reference run).
3. Divide by the macro wall time.

This is an estimate; it assumes the micro-benchmark's per-call cost transfers to the
real run. **It was validated once**: for mutation it predicted 8.5%, the profiler measured
7.6%.

A profile is the better source when one is available, and it is cheap to collect.

### Predicting an end-to-end gain

With the step's share `s` and the measured speed ratio `r` of the optimised step:

```
new_total / old_total = (1 - s_old) + s_old * r
```

where `s_old` is the share **before** the change. If only the post-change share `s_new`
is known, `s_old = s_new / r * (…)` — easier to work from the post-change numbers:

```
old_total / new_total = 1 - s_new + s_new / r
```

**Worked example, in-place mutation**: `s_new = 0.076`, `r ≈ 0.30` ->
`old/new = 1 - 0.076 + 0.253 = 1.177`, i.e. a predicted **15.0%** gain.
The measured gain was **22.3%**. The formula under-predicts for the reason given above:
it does not carry the deferred GC cost. Treat it as a **lower bound** when the change
removes allocations that survive gen0.

### 2026-09-30 — try/catch removed from EntityCrossover

Runs (ms): 13 142 / 13 163 / 13 054 → mean **13 120**. Entities and best distance bit-identical
(9,006,240 / 7849.395176892104), which is the evidence that the catch never fired on this path —
the user had also confirmed it with a breakpoint.

The gain (−4.6%) is larger than the crossover's 4.8% share would suggest for a change that removes
no work at all. Likely cause: a `try` region constrains JIT code motion and inlining inside a hot
inner loop. Not profiled, so treat the explanation as a hypothesis; the measurement itself is
outside the ~1% run-to-run noise and was reproduced three times.

Worth remembering: a swallow-everything `catch` in a hot loop can cost measurable time even when
it never catches anything.

### 2026-10-07 — crossover parent-instance guard

Interleaved A/B in the same session (A = current tree with the guard call disabled in a scratch copy,
B = current tree), Elapsed ms:

- A: 13 276 / 13 413 / 13 635 → mean **13 441**
- B: 13 327 / 13 631 / 13 649 → mean **13 536**

Difference +95 ms (+0.7%), inside the ~±1% run-to-run noise; result bit-identical in all runs
(9,006,240 entities, 7849.395176892104). The guard is 2–4 `ReferenceEquals` per crossover call.

Note: both arms are ~2.5% slower than the 2026-09-30 figure (13 120 ms). Since arm A has no guard,
the shift is not caused by it; it comes from the environment or from other changes made since then
(fitness-comparer interface changes). Not isolated further — a checkout of the 09-30 tree would be needed.
Lesson: compare against an A/B taken in the same session, not against an older logged number.

### 2026-10-07 — cached chromosome names in EntityCrossover

Measured after a reboot, interleaved A/B (A = current tree with the old `foreach` over
`parents[0].Chromosomes.Keys` restored in a scratch copy, B = current tree), Elapsed ms:

- A: 14 285 / 13 894 / 14 342 / *19 142* → mean of first three **14 174**
- B: 14 029 / 14 133 / 14 686 / *15 800* → mean of first three **14 283**

The 4th pair is a background-load spike hitting both arms (+35% / +12%) and is excluded. Difference
+0.8%, within noise; result bit-identical. Expected: the change removes one boxed enumerator per
`Cross` call, small next to the rest of the work. Its purpose is correctness (no enumeration of a live
`IDictionary`, safe on netstandard2.1 runtimes with throw-on-modify enumerators), not speed.

Environment note: the whole level moved from ~13.4 s to ~14.2 s between sessions on the same code,
even after a reboot (post-boot background activity: explorer/OneDrive, total CPU 7–29% while idle).
Absolute numbers across sessions are not comparable on this machine; only same-session A/B is.

## Second reference workload: SiteArranger (SAD)

A real consuming application, not an example. `SiteArranger.DirectMessageTestConsoleApp`
(`c:/Git/SiteArranger_Benchmark`), which references this repo's `PEA.csproj` directly.

Deterministic for the same reason as the TSP example: the request carries `seed: 0`, and
`ArrangerMain` substitutes the constant `20220901` "for reproducibility". The stop criterion
is `IterationsReached(500000 * cuboidsCount)` — **iteration-based, not time-based** (the
`TimeoutElapsed` call on the same line is commented out), confirmed by measurement: six runs
produced exactly 7,008,400 evaluated arrangements while wall time varied.

Two inputs, both in the app's `bin` folder:

| Request | Shape | Iterations |
|---|---|---|
| `271148` | 7 cuboids, 2 exclude zones, 8 connections — small greenfield | 3.5M |
| `270660` | 11 cuboids, 20 exclude zones, 99 connections — heavy | 5.5M |

### 2026-09-30 — in-place mutation + try/catch removal, measured on SAD `271148`

| | Runs (ms) | Mean | Arrangements |
|---|---|---:|---:|
| before (HEAD `4a4397c`) | 29 199 / 29 465 / 29 228 | **29 297** | 7 008 400 |
| after | 27 633 / 27 269 / 27 024 | **27 309** | 7 008 400 |

**−6.8%** on identical work.

**Why this is much less than the TSP example's −25.8%.** Per evaluated entity the saving is
**0.51 µs on TSP** (4 573 ms / 9,006,240) but **0.28 µs on SAD** (1 988 ms / 7,008,400), so it is
not the same in absolute terms either — the two runs use different chromosome types and operators
(TSP: one `PermutationChromosome`; SAD: `DoubleVectorChromosome`s), so the removed clone and the
removed `try` cost differently per entity. On top of that SAD's evaluation (geometry, collision and
boundary checks) is far more expensive than a 52-point tour length, so the same kind of saving is a
smaller share of the total. The framework's share of SAD's runtime was **not** measured here; the
profile of the heavy request `270660` (see the Pea.Geometry analysis) put
`CalculatePenaltyForOutsideOfSiteBoundaries` alone at ~90% of the loop, so for that input the
evaluation dominates.

*Correction 2026-10-07: an earlier version of this paragraph claimed an equal absolute gain and a
framework share of "roughly a quarter", and listed 99 connections for `271148` (that is `270660`).
None of that was measured; replaced with the figures above.*
