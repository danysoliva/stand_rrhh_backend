namespace RRHH_WEB_API._Common
{
    public sealed class Response<TData>
    {

        Response()
        {

        }

        public TData Data { get; private set; }
        public string Message { get; private set; }
        public AnswerTypeEnum AnswerType { get; private set; }
        public bool Ok => AnswerType == AnswerTypeEnum.Ok;

        public static Response<TData> Success(TData data)
        {
            return new Response<TData>
            {
                AnswerType = AnswerTypeEnum.Ok,
                Data = data,
            };
        }

        public static Response<TData> Validation(string mensaje)
        {
            return new Response<TData>
            {
                AnswerType = AnswerTypeEnum.Warning,
                Message = mensaje
            };
        }

        public static Response<TData> Excepcion(string mensaje)
        {
            return new Response<TData>
            {
                AnswerType = AnswerTypeEnum.Error,
                Message = mensaje
            };
        }

    }

    public enum AnswerTypeEnum
    {
        Ok = 1,
        Warning = 2,
        Error = 3
    }
}
