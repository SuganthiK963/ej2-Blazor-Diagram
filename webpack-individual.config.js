const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const env = process.env.NODE_ENV || 'production';

const commonConfig = {
  context: __dirname + "/Scripts",
  target: 'web',
  entry: JSON.parse(fs.readFileSync(__dirname + '/Scripts/individual-entries.json', 'utf8')),
  output: {
    filename: '[name]-[hash].min.js',
    hashDigestLength: 6
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: path.resolve(__dirname, '/Scripts/modules/'),
        use: {
          loader: 'babel-loader'
        }
      }
    ]
  },
  plugins: [
    new webpack.NamedModulesPlugin()
  ],
  optimization: {
    usedExports: true
  }
};

if (env === 'development') {
  commonConfig.mode = 'development';
  module.exports = merge(commonConfig, {
    devtool: 'none',
    devServer: {
      contentBase: './Scripts',
      publicPath: '/',
      historyApiFallback: true,
      port: 3000
    },
  });
}

if (env === 'production') {
  commonConfig.mode = 'production';
  // commonConfig.plugins.push(new BundleAnalyzerPlugin());
  module.exports = merge(commonConfig, {});
}