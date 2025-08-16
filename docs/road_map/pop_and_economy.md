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
- [ ] **Create a basic unique market system**
  - [x] All players can access the market regardless of location
  - [ ] Market should keep track of at least one good, the quantity available, quantity needed and it's price
  - [ ] Keeping  trace of good price through time
  - [ ] All goods must be defined in a json on load at start

- [ ] **Create a Basic pop system Model**
  - [X] pop should have a population size
  - [X] pop should have good needs
  - [ ] pop should be able to buy good on market
  - [ ] pop should grow or shrink based on if they could buy good or not 


### Ideas for Future Iterations

- Support multiple markets, with at least one by country.
- Allow markets to trade with each other:
  - Limited inter-market exchange capacity.
  - Limited buying capacity by transport mode (ground, maritime, air).
  - Global limitations, e.g.:
    - Maximum 30k overall.
    - Or 20k limit for ground transport with Market 1 and Only maritime capacity available with Market 2.
    
 - factory and good consumption and production
