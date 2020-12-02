# VulnerabilitiesDB
## NYSS course lab 2
Program that fetch information from remote resource and create local storage, show it as table in main window.  
## Instruction
At start program is trying to find local saved data. If succed it will load date and open main window.
If there is no local saved data it will ask you if you want to download data. 
If you'll agree it'll download xlsx file, transform it to usable inner format and make a binary save on local storage.
If you reject download it'll open main window.  
At main window you have main table with short info about vulnerabilities. For detailed info you can click on them and it'll show all information about one you choose.
Paging was adjusted here, so you can select amount of record per page and switch between pages by navigation bar below table.
At last on main window you can find button for force update that will download remote data, analize it, update current information and show you brief report which iclude success status, amount of updated records and diff between previous and current states. At diff table you can see exact record's ID, field that was changed and how it looked before and after update. 
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
