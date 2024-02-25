# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.2] - 2024-02-25

- `UnityRandomSource` is now used for `IRandomRangeSource.Default`
- Add runtime assertion for custom `RandomSource` constraints: Source must provide a value between minInclusive
  and maxExclusive, unless min is equal to max, then min should be returned.

## [1.0.1] - 2024-02-11

- Lower the minimum Unity version to 2021.3
- Improve Unity sample

## [1.0.0] - 2024-02-10

- Initial release
