# MinifyMe

A simple console app to minify CSS/JS files on the file system

## Config.json example

	{
		"minify_css": true,
		"minify_js": true,
		"js_files": [
			"scripts/",
			"quotenet/*",
			"api.js"
		],
		"css_files": [
			"css/*"
		]
	}

	
 - * wildcard makes app go through all subdirectories for files
 - If its a folder "/" then itll minify all files within that folder
 - Single files can be specified as well