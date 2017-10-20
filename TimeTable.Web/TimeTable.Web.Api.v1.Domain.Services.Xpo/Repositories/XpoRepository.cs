using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    public abstract class XpoRepository
    {
        protected readonly DevExpress.Xpo.Session session;

        protected XpoRepository(DevExpress.Xpo.Session session)
        {
            this.session = session ??
                throw new ArgumentNullException(nameof(session));
        }
    }
}