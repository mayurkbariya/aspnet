namespace FBDropshipper.Common.Requests
{
    public class PageRequestModel
    {
        private string _search = "";
        private string _orderBy = "CreatedDate";

        public string Search
        {
            get => _search;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                _search = value.ToLower();
            }
        }

        public bool IsDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string OrderBy
        {
            get => _orderBy;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                _orderBy = value;
            }
        }

        public virtual void SetDefaultValue()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Search = "";
            }

            OrderByFilter();
        }

        public virtual void OrderByFilter()
        {
            if (string.IsNullOrWhiteSpace(OrderBy))
            {
                OrderBy = "CreatedDate";
            }
        }
    }
}