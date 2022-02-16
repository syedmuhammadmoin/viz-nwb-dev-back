﻿using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        public IClientRepository Client { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IOrganizationRepository Organization { get; private set; }
        public IBusinessPartnerRepository BusinessPartner { get; private set; }
        public IProductRepository Product { get; private set; }
        public ILevel4Repository Level4{ get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Client = new ClientRepository(context);
            Organization = new OrganizationRepository(context);
            Category = new CategoryRepository(context);
            BusinessPartner = new BusinessPartnerRepository(context);
            Level4 = new Level4Repository(context);
            Product = new ProductRepository(context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
