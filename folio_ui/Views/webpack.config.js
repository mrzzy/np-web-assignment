/*
 * webpack configuration
 * NP Web Assignment
*/

const path = require("path");
const dotenv = require("dotenv-webpack");
 
module.exports = {
    // client scripts point 
    entry: path.resolve(__dirname, "index.js"),
    // output configuration
    output: {
        path: path.resolve(__dirname, "..", "wwwroot"),
        publicPath: '/',
        filename: path.join("js", "bundle.js")
    },
    // webpack module
    module: {
        rules: [
            // babel
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: [ 'babel-loader' ]
            },
            // css
            { 
                test: /\.css$/,
                use: [ 'style-loader','css-loader' ]
            },
            // images
            {
                test: /\.(png|jpe?g|gif)$/i,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name:'[name].[ext]',
                        outputPath: "img"
                    },
                }],
            }
        ]
    },
    // unbundled modules
    externals: {
        jquery: '$',
        bootstrap: '$'
    },
    resolve: {
        extensions: [".js", ".json"]
    },
    // plugins
    plugins: [ 
        new dotenv()
    ]
}; 
