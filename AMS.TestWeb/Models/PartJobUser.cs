using System;
using System.Collections.Generic;
using System.Text;

namespace Jerrisoft.Platform.TestWeb
{
    public class PartJobUser
    {
        public string Id { get; set; }
        public string FkOrgId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PostionName { get; set; }
    }

    public class ActivityType
    {
        public string Id { get; set; }
        public string ActivityTypeName { get; set; }

    }
}
