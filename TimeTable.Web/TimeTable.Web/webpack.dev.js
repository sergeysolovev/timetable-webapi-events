﻿const path = require('path');
const autoprefixer = require('autoprefixer');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const ManifestPlugin = require('webpack-manifest-plugin');

const publicPath = process.env.PUBLIC_PATH || '/timetable/';

const config = {
  devtool: 'cheap-module-source-map',
  entry: {
    main: path.resolve(__dirname, 'src/index')
  },
  output: {
    filename: '[name].js',
    chunkFilename: '[name].js',
    publicPath
  },
  devServer: {
    port: 8080,
    contentBase: false,
    compress: true,
    quiet: false,
    inline: true,
    lazy: false,
    host: "0.0.0.0",
    headers: {
      "Access-Control-Allow-Origin": "*"
    }
  },
  resolve: {
    alias: {
      jquery: "jquery/src/jquery",
    }
  },
  module: {
    rules: [
      {
        test: require.resolve('jquery'),
        use: [
          'imports-loader?this=>window',
          'imports-loader?define=>false'
        ]
      },
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader'
        }
      },
      {
        test: /fonts\.css$/,
        use: {
          loader: 'css-loader'
        }
      },
      {
        test: /\.(css|less)$/,
        exclude: /fonts\.css$/,
        use: ExtractTextPlugin.extract({
          fallback: 'style-loader',
          use: [
            {
              loader: 'css-loader'
            },
            {
              loader: 'less-loader',
              options: {
                paths: [
                  path.resolve(__dirname, 'node_modules')
                ]
              }
            },
            {
              loader: 'postcss-loader',
              options: {
                ident: 'postcss',
                plugins: () => [
                  require('postcss-flexbugs-fixes'),
                  autoprefixer({
                    browsers: [
                      '>1%',
                      'last 4 versions',
                      'Firefox ESR',
                      'not ie < 9',
                    ],
                    flexbox: 'no-2009',
                  }),
                ],
              },
            }
          ]
        })
      },
      {
        test: /\.(ico|svg|woff|woff2|eot|ttf|otf)$/,
        use: {
          loader: 'file-loader',
          options: {
            name: 'static/media/[name].[ext]'
          }
        }
      }
    ]
  },
  plugins: [
    new webpack.DefinePlugin({
      'process.env': {
        'NODE_ENV': JSON.stringify('development'),
        'process.env.PUBLIC_PATH': JSON.stringify(publicPath)
      }
    }),
    new webpack.HashedModuleIdsPlugin(),
    new webpack.optimize.CommonsChunkPlugin({
      async: 'vendor',
      children: true
    }),
    new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
    new webpack.ProvidePlugin({
      $: 'jquery',
      jQuery: 'jquery',
      'window.jQuery': 'jquery'
    }),
    new ExtractTextPlugin({
      filename: '[name].css',
      allChunks: true
    }),
    new ManifestPlugin({
      fileName: 'asset-manifest.json'
    })
  ]
};

module.exports = config;
