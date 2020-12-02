# VulnerabilitiesDB
## NYSS course lab 2
Program that fetch information from remote resource and create local storage, show it as table in main window.  
## ~~Add casual explanation~~  
## Required funcitonality
* [x] Fetch information from remote server and create local base of vulnurabilities with next fields
  * ID
  * Name
  * Description
  * Source
  * Vulnurability's object 
  * Violation of CIA
* [x] Auto update on request
  * [x] Success status
  * [x] Present diff of data and total of new records
* [x] Present information as table with fields
  * ID (ex. "УБИ.123")
  * Name
* [x] Pagination of data presentation 
  * [x] Choise for amount of records per page
  * [x] No less than 15 records per page
* [x] Can see full information for each record
* [x] Can save data as file on drive
  * [x] On start program should check if there is saved data
  * [x] Notify if there is no saved data and ask to start download
