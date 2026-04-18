namespace DAL_Celebrity
{
    public interface IRepository<T1, T2> : IMix<T1, T2>, ICelebrity<T1>, ILifeevent<T2> { }

    public interface IMix<T1, T2>
    {
        List<T2> GetLifeeventsByCelebrityId(int celebrity_id);
        T1? GetCelebrityByLiveeventId(int liveevent_id);
    }

    public interface ICelebrity<T1> : IDisposable
    {
        List<T1> GetAllCelebrities();
        T1? GetCelebrityById(int celebrity_id);
        bool AddCelebrity(T1 celebrity);
        bool DelCelebrity(int celebrity_id);
        bool UpdCelebrity(int celebrity_id, T1 celebrity);
        int GetCelebrityIdByName(string celebrity_name);
    }

    public interface ILifeevent<T2> : IDisposable
    {
        List<T2> GetAllLiveevents();
        T2? GetLiveeventsById(int liveevent_id);
        bool AddLiveevent(T2 liveevent);
        bool DelLiveevent(int liveevent_id);
        bool UpdLiveevent(int liveevent_id, T2 liveevent);
    }
}