using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    ///<summary>This class extends <c>Response</c> and represents the result of a call to a non-void function. 
    ///In addition to the behavior of <c>Response</c>, the class holds the value of the returned value in the variable <c>Value</c>.</summary>
    ///<typeparam name="T">The type of the returned value of the function, stored by the list.</typeparam>
    public class Response<T> : Response
    {
        public T ReturnValue { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorMessage {get; set;}  
        public Response(string ErrorMessage) : base(ErrorMessage) { }
        public Response(T ReturnValue) : base()
        {
            this.ReturnValue = ReturnValue;
        }
        [JsonConstructor]
        public Response(string ErrorMessage, T ReturnValue) : base(ErrorMessage)
        {
            this.ReturnValue = ReturnValue;
        }
    }
}
