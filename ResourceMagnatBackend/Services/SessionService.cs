using ResourceMagnat.Data;
using ResourceMagnat.Models;

namespace ResourceMagnat.Services
{
    public class SessionService
    {
        private readonly MainDbContext context;

        public SessionService(MainDbContext _context)
        {
            context = _context;
        }

        public void ClearEndedSessions()
        {
            // Удаляем завершённые сеансы
            var endedSessions = context.Sessions.ToList().Where(i => (DateTime.Now - i.LastAccess).TotalSeconds >= Config.SESSION_LENGTH_IN_SECONDS);
            foreach (var endSession in endedSessions)
            {
                context.Sessions.Remove(endSession);
            }
            context.SaveChanges();
        }

        public int? GetUserIdBySession(string sid)
        {
            ClearEndedSessions();

            var session = context.Sessions.FirstOrDefault(i => i.Id == sid);
            if (session != null)
            {
                // ОЛбновляем время активности сессии
                session.LastAccess = DateTime.Now;
                context.SaveChanges();
            }
            return session?.UserId;
        }

        public string CreateSession(User user)
        {
            ClearEndedSessions();

            // Создаём сеанс
            var existSesion = context.Sessions.FirstOrDefault(i => i.UserId == user.Id);
            if (existSesion == null)
            {
                existSesion = new Session
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    LastAccess = DateTime.Now
                };
                context.Sessions.Add(existSesion);
                context.SaveChanges();
            }
            else
            {
                existSesion.LastAccess = DateTime.Now;
                context.SaveChanges();
            }
            return existSesion.Id;
        }
    }
}
