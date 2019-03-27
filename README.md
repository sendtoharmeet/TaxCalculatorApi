#It was hard but challenging task for me. XML can be easily parsed with DTD approach and 'Embedded XML' cannot directly parsed, 
 So I did not follow XMLDocument or XMLElement classes of .NET framework,
 Rather I choose my custom logic to parse this type of data which seems me easy and flexible approach.

#Although there are more logic needs to be written for example, what if I have multiple similar XML tags, 
#I can handle that as well and in that case a same list of multiple objects will be returned.

## But I hope for interview purpose, it should be enough.

I tried to cover all desired aspects (design, reliability, readability, extensible, quality) and implemented as following: 

#Implement "SOLID" principles to make it flexible and extensible. Every class is independent and created for its own purpose. No dependency.
#Implemented interface architect which will support repository pattern in future to call DB layer and Unit test cases with dependency injection at constructor level
#Implemented dependency on constructor level, use of "Unity" to resolve dependency. 
	## It gives a flexibility to implement or inject new classes without much change in code.
#Implemented Multiplayer in project to make every part independent.
	## Service-Layer between UI & DataAccess(Not introduced yet), & Service layer currently playing validation & processing business logic,
	## UnitTest cases project is created to implement unit test cases. Currently created tests for API project, 
	   We can create Service Unit test as well that will based on Service project



Note: I implemented best standards of industry and now we can use this project as a basic structure or architect to enhance this module.

Still if you think, I could do much better please let me know the proper feedback. That will help me to enhance my skills as a Software developer.

I would be thank full to you and would like to hearing from you soon.
