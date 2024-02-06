# gulp-less-vars-to-js [![Build Status](https://travis-ci.org/vagusX/gulp-less-vars-to-js.svg?branch=master)](https://travis-ci.org/vagusX/gulp-less-vars-to-js)

> My amazing gulp plugin


## Install

```
$ npm install --save-dev gulp-less-vars-to-js
```


## Usage

```js
const gulp = require('gulp');
const lessVarsToJs = require('gulp-less-vars-to-js');

gulp.task('default', () =>
	gulp.src('src/file.ext')
		.pipe(lessVarsToJs())
		.pipe(gulp.dest('dist'))
);
```


## API

### lessVarsToJs([options])

#### options

Type: `Object`

##### foo

Type: `boolean`<br>
Default: `false`

Lorem ipsum.


## License

MIT © [vagusX](http://github.com/vagusX/gulp-less-vars-to-js)
