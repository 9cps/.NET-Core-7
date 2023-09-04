namespace Application.Model
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        //public decimal Total { get; set; }
        public object Data { get; set; }
        public DateTime Date { get; } = DateTime.Now;
    }
}
