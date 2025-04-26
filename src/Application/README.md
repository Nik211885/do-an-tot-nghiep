## Think about application layer


In application layer is defined services for Persistence layer want to use abstraction layer to services,
and it has implement in inf layer

What do you think about problem is pagination it has in application services because core layer 
don't need page size and page number in ui layer want

But some time handler just call services add don't add any logic.
What do you think about every service is disposed in Persistence or just is handler it makes complex code,
but it makes simple design and easy test, and it makes not good for performance application


### Problem about think

I want to make don't have dependency redundant in between layer like in domain and application I don't expose queryable
it dependency to linq and ef I know you convert expression to sql and still query in database but in my case I don't like make this


## Concept about module

It encapsulation to folder it makes to easy fun file release make edit