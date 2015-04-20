using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Generic.TabularData
{
    [Serializable]
    public class TabularColumn
    {
        public string id { get; set; }
        public string lable { get; set; }        

        [Newtonsoft.Json.JsonIgnore]
        public Type DataType { get; set; }
        
        /// <summary>
        /// Function to transform cell value for display. Input parameter to Func(object cell) is cell value
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Func<object, string> CellValueFormatter { get; set; }

        public TabularColumn(string lable)
            : this(lable, typeof(string))
        {            
        }

        public TabularColumn(string lable, Type type)
        {
            this.lable = lable;         
            this.DataType = type;
        }

        public TabularColumn(string id, string lable) : this(id, lable, typeof(string))
        {
        }

        public TabularColumn(string id, string lable, Type type) : this (lable, type)
        {
            this.id = id;            
        }
    }
}