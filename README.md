# MinifyMe

A simple console app to minify CSS/JS files on the file system

## Config.json example

{
	"minify_css": true,
	"minify_js": true,
	"minify_rename": "*.min.*",
	"js_files": [
		"scripts/",
		"quotenet/*"
	],
	"css_files": [
		"css/*.css"
	]
}