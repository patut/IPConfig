# IP Location Project
To run an application, please, run solution in the Visual Studio and open the link in browser https://localhost:8080/index.html

The frontend part contains a lot of files. In realty js files should all be merged and minified into 1 file using webpack or other tool, but I decided to keep them seperately for better readability of the code.

Parsing all data immediately during the application bootstrap did not produce acceptable results in terms of time limit on the used testing machine. That is why lazy parsing of the records was implemented.


