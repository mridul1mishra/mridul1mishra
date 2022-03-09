using FX.Core.Models.Form;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FX.Core.Models.Form
{
    public class FormReporting
    {
        public List<SelectListItem> Forms { get; set; }
        public string SelectedForm { get; set; }
        public string SelectedFormValue { get; set; }
        public string FromDateRange { get; set; }
        public string FromDateRangeValue { get; set; }
        public string ToDateRange { get; set; }
        public string ToDateRangeValue { get; set; }

        public FormReporting()
        {
            Forms = new List<SelectListItem>() { new SelectListItem() { Text = "select the form", Value = "0", Selected = true } };
        }
    }
    
    public class FormReportingParams
    {
        public string SelectedFormValue { get; set; }
        public string FromDateRangeValue { get; set; }
        public string ToDateRangeValue { get; set; }
    }

    public class FormReportingData
    {
        public DataTable FormDataTable { get; set; }
        public FormReportingData()
        {
            FormDataTable = new DataTable();
        }
    }

    public class FormDataFormat
    {
        public WffmFormParams WffmFormData { get; set; }
        public string FormId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Language { get; set; }

        public FormDataFormat(string formId, DateTime dateTime, string language)
        {
            WffmFormData = new WffmFormParams();
            FormId = formId;
            CreatedDate = dateTime;
            Language = language;
        }
    }
}