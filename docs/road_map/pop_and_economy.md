## Dev country and province Iteration Overview
Back to [Main read me](../../README.md)
Back to [Road Map](../roadmap.md)

### Todo: Iteration 01
The first iteration focuses on the minimum viable way to represent province and country.
- [x] **Create a good object**
  - [x] name
  - [x] Type
  - [x] icon
  - [x] weight
  - [x] base price
- [x] **Create a market_good object**
  - [x] public int id;
  - [x] public Good good;
  - [x] public float supply;
  - [x] public float demand;
  - [x] public float price;
- [x] **Create a basic global market system**
  - [x] All players can access the market regardless of location
  - [x] Market should keep track of at least one good, the quantity available, quantity needed and it's price
  - [x] Keeping  trace of good price through time
  - [x] Keeping  trace of supply through time
  - [x] Keeping  trace of demand through time
  - [x] Price should vary with supply and demand

- [x] **Create a Basic pop system Model**
  - [X] pop should have a population size
  - [X] pop should have good needs
  - [X] pop should be able to buy good on market
  - [X] pop should be able to sell good on market
  - [X] pop should grow or shrink based on if they could buy good or not 
  - [x] pop stockpile reset on first day of month 


### Ideas for Future Iterations

- refactor with getter and setter !
- pop and market supply, demand, cash ammount RoundToTwoDecimals when set
- All goods must be defined in a json on load at start
- Support multiple markets, with at least one by country.
- Allow markets to trade with each other:
  - Limited inter-market exchange capacity.
  - Limited buying capacity by transport mode (ground, maritime, air).
  - Global limitations, e.g.:
    - Maximum 30k overall.
    - Or 20k limit for ground transport with Market 1 and Only maritime capacity available with Market 2.
    
 - factory and good consumption and production
 - pop Need tier (low, middle , high)
 - goods that allow pop to produce more
