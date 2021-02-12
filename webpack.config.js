module.exports = env => {
    return {
        entry: {worker: ['./tmp/Worker.js']},
        context: __dirname,
        target: 'webworker',
        output: {
            path: __dirname + '/workers-site'
        },
        mode: "production",
        resolve: {
            // See https://github.com/fable-compiler/Fable/issues/1490
            symlinks: false
        },
        plugins: [ ],
        module: {
            rules: [ ]
        }
    }
}

