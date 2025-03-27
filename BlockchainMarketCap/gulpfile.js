const gulp = require("gulp");
const sass = require("gulp-sass")(require("sass"));
const webpack = require("webpack-stream");
const browserSync = require("browser-sync").create();
const clean = require("gulp-clean");
const { exec } = require("node:child_process");
const { VueLoaderPlugin } = require("vue-loader");

// Clean dist directory
gulp.task("clean", () => {
	return gulp
		.src("wwwroot/css", { read: false, allowEmpty: true })
		.pipe(clean());
});

// Compile SCSS files to CSS
gulp.task("scss", () => {
	return gulp
		.src("wwwroot/scss/**/*.scss")
		.pipe(sass().on("error", sass.logError))
		.pipe(gulp.dest("wwwroot/css"))
		.pipe(browserSync.stream());
});

// Bundle TypeScript and Vue files with webpack
gulp.task("webpack", () => {
	return gulp
		.src("wwwroot/ts/main.ts")
		.pipe(
			webpack({
				mode: "development",
				devtool: "source-map",
				output: {
					filename: "bundle.js",
				},
				resolve: {
					extensions: [".ts", ".js", ".vue"],
				},
				module: {
					rules: [
						{
							test: /\.ts$/,
							loader: "ts-loader",
							options: {
								appendTsSuffixTo: [/\.vue$/],
							},
						},
						{
							test: /\.vue$/,
							loader: "vue-loader",
						},
						{
							test: /\.css$/,
							use: ["vue-style-loader", "css-loader"],
						},
					],
				},
				plugins: [new VueLoaderPlugin()],
			}),
		)
		.pipe(gulp.dest("wwwroot/js"))
		.pipe(browserSync.stream());
});

// Start the .NET application
gulp.task("dotnet", (done) => {
	const dotnetProcess = exec("dotnet run", { cwd: "./" });

	dotnetProcess.stdout.on("data", (data) => {
		console.log(data);
		// Look for the URL in the output to determine when the app is ready
		if (data.includes("https://localhost")) {
			const url = data.match(/https:\/\/localhost:[0-9]+/)[0];
			setTimeout(() => {
				browserSync.init({
					proxy: url,
					open: false,
				});
			}, 3000); // Give the server a moment to initialize
		}
	});

	dotnetProcess.stderr.on("data", (data) => {
		console.error(data);
	});

	done();
});

// Watch for file changes
gulp.task("watch", () => {
	gulp.watch("wwwroot/scss/**/*.scss", gulp.series("scss"));
	gulp.watch(
		["wwwroot/ts/**/*.ts", "wwwroot/ts/**/*.vue"],
		gulp.series("webpack"),
	);
	gulp.watch(["**/*.razor", "**/*.cshtml"], (done) => {
		browserSync.reload();
		done();
	});
});

// Default task
gulp.task(
	"default",
	gulp.series("clean", gulp.parallel("scss", "webpack"), "dotnet", "watch"),
);

// Build task (without running the app)
gulp.task("build", gulp.series("clean", gulp.parallel("scss", "webpack")));
