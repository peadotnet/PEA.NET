using System.Collections.Generic;

namespace Pea.Core
{
	public interface IReduction
	{
		IList<IEntity> Reduct(IList<IEntity> entities);
	}
}
