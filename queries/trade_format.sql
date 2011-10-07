   declare @result varchar(256)
   exec dbo.place_sell_order 'enmaku2', 10.00000000, 10.00000000, @result output
   select @result
   
   declare @result varchar(256)
   exec dbo.place_buy_order 'enmaku', 10.00000000, 15.00000000, @result output
   select @result   
   
   select * from dbo.sell where filled = 0
   select * from dbo.buy where filled = 0
   select * from dbo.trades
   
   
   delete from dbo.trades
   delete from dbo.sell
   delete from dbo.buy
   
   declare @balance decimal(38,8)
   exec dbo.get_balance 'enmaku', 'btc', @balance output
   select @balance
   
   select * from dbo.balances
   
   delete from dbo.balances
   select * from dbo.deposits
   delete from dbo.deposits
   exec dbo.add_deposit 13, 'usd', 1000
   exec dbo.finalize_deposit 38