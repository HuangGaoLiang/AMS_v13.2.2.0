using Jerrisoft.Platform.Logic;

namespace AMS.Core
{
    public class BService : BaseService
    {
        public override byte BussinessId
        {
            get
            {
                return BusinessConfig.BussinessID; 
            }
        }
    }
}
