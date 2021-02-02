public class BaseSession : IBaseSession
{
    public BaseSession()
    {

    }
    public BaseModel _BaseModel
    {
        get
        {
            return SessionRequest._User;
        }
        set
        {
            this._BaseModel = value;
        }
    }


}
