var gulp = require("gulp"),
    less = require("gulp-less"),
    concat = require("gulp-concat"),
    lessToVars = require('gulp-less-vars-to-js'),
    rename = require('gulp-rename'),
    replace = require('gulp-replace'),
    uglify = require('gulp-uglify');

// common.js
gulp.task('bundle-common-js', function () {
    return gulp.src(["Scripts/Common/*.js"])
        .pipe(concat('common.js'))
        .pipe(uglify().on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest('./wwwroot/clothes-shop/js'));
});


// workerLayout.js
gulp.task('bundle-workerLayout-js', function () {
    return gulp.src(["Scripts/Pages/WorkerLayout/*.js"])
        .pipe(concat('workerLayout.js'))
        .pipe(uglify().on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest('./wwwroot/clothes-shop/js'));
});

// adminLayout.js
gulp.task('bundle-adminLayout-js', function () {
    return gulp.src(["Scripts/Pages/AdminLayout/*.js"])
        .pipe(concat('adminLayout.js'))
        .pipe(uglify().on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest('./wwwroot/clothes-shop/js'));
});

// mainLayout.js
gulp.task('bundle-mainLayout-js', function () {
    return gulp.src(["Scripts/Pages/MainLayout/*.js"])
        .pipe(concat('mainLayout.js'))
        .pipe(uglify().on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest('./wwwroot/clothes-shop/js'));
});

// all js
gulp.task('bundle-process-js', gulp.series(
    'bundle-common-js',
    'bundle-mainLayout-js',
    'bundle-adminLayout-js',
    'bundle-workerLayout-js',
));
