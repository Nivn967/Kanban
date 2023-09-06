using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object ReturnValue { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool ErrorOccured { get => ErrorMessage != null; }

        public Response() { }

        public Response(string ErrorMessage) //, object ReturnValue
        {
            this.ErrorMessage = ErrorMessage;
            //this.ReturnValue = ReturnValue;
        }
        [JsonConstructor]
        public Response(string ErrorMessage, object ReturnValue) //, object ReturnValue
        {
            this.ErrorMessage = ErrorMessage;
            this.ReturnValue = ReturnValue;
        }
    }
}