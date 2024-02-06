const PluginError = require('plugin-error');
const through = require('through2');
const lessToJs = require('less-vars-to-js');

const pluginName = require('./package.json').name;

const defaultOpts = {
  resolveVariables: true,
  stripPrefix: true,
};

module.exports = (opts) => {
  opts = opts || {};

  return through.obj(function(file, enc, callback) {
    if (file.isNull()) {
      callback(null, file);
      return;
    }

    if (file.isStream()) {
      callback(new PluginError(pluginName, 'Streaming not supported'));
      return;
    }

    const options = { ...defaultOpts, ...opts };

    try {
      const compiledLessVarsText = lessToJs(file.contents.toString(), options);
      file.contents = Buffer.from(`module.exports = ${JSON.stringify(compiledLessVarsText, null, 2)}`);
      this.push(file);
    } catch (error) {
      this.emit(
        'error',
        new PluginError(pluginName, error, {
          fileName: file.path,
          showProperties: false,
        }),
      );
    }
    callback();
  });
};
