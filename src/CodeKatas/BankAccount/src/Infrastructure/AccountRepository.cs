﻿using BankAccount.Domain.Accounts;
using BankAccount.Domain.Accounts.Memento;
using BankAccount.Domain.Accounts.Repository;
using Zero.DataBase;
using Zero.Domain;

namespace BankAccount.Infrastructure;

public class AccountRepository : IAccountRepository
{
    private readonly ZeroDbContext _context;

    public AccountRepository(ZeroDbContext context)
    {
        _context = context;
    }


    public async Task<Account> Reconstitute(AccountId id)
    {
        var accountMemento = await _context.Set<AccountMemento>()
            .FindAsync(id.Id);

        var reconstitute = Account.Reconstitute(accountMemento);
        
        return reconstitute;
        Queue<IsADomainEvent> events;
        return Account.Reconstitute(events);
        // TODO 
        //await _context.SaveChangesAsync();
    }

    public async Task Store(Account account)
    {
        await _context.Set<AccountMemento>()
            .AddAsync(account.TakeMemento());

        // TODO 
        //await _context.SaveChangesAsync();
    }
}