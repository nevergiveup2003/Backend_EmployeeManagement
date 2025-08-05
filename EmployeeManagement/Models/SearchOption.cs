﻿namespace EmployeeManagement.Models
{
    public class SearchOptions
    {
        public string? Search { get; set; }
        public int? PageIndex { get; set; } 
        public int? PageSize { get; set; } = 10;
    }
    public class PageData<T>
    {
        public int TotalData { get; set; }
        public List<T> Data { get; set; } = new List<T>();
    }
}
