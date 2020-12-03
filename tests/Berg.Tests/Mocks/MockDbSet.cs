using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Berg.Tests.Mocks {
    public class MockDbSet<TEntity> : Mock<DbSet<TEntity>> where TEntity : class {

        public ICollection<TEntity> Store { get; set; }
        public IQueryable<TEntity> Query { get; set; }

        public MockDbSet(ICollection<TEntity> data = null) {
            Store = data ?? new List<TEntity>();
            Query = Store.AsQueryable();

            As<IQueryable<TEntity>>().SetupGet(x => x.Provider).Returns(Query.Provider);
            As<IQueryable<TEntity>>().SetupGet(x => x.Expression).Returns(Query.Expression);
            As<IQueryable<TEntity>>().SetupGet(x => x.ElementType).Returns(Query.ElementType);
            As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(Query.GetEnumerator());

            Setup(x => x.Add(It.IsAny<TEntity>())).Callback((TEntity entity) => Store.Add(entity));
            Setup(x => x.Attach(It.IsAny<TEntity>())).Callback((TEntity entity) => Store.Add(entity));
            Setup(x => x.Remove(It.IsAny<TEntity>())).Callback((TEntity entity) => Store.Remove(entity));

            Setup(x => x.Find(It.IsAny<It.IsAnyType>()))
                .Throws(new NotImplementedException("Implement find method according to the generic type"));

        }
    }
}
