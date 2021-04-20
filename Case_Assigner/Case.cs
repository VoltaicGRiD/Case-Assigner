using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case_Assigner
{
    public class Case
    {
        List<Case> CaseList = new List<Case>();

        public string TimeQueued { get; set; }
        public string SR { get; set; }
        public string SiteName { get; set; }
        public string Product { get; set; }
        public string UnifiedSupport { get; set; }
        public string Country { get; set; }

        public void InsertCase(string[] data)
        {
            CaseList.Add(new Case() { TimeQueued = data[0], SR = data[1], SiteName = data[2], Product = data[3], UnifiedSupport = data[4], Country = data[5] });
        }

        public List<Case> GetCases()
        {
            return CaseList;
        }
    }
}