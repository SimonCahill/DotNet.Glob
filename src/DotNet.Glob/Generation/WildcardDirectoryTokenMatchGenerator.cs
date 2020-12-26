﻿using System;
using System.Text;
using DotNet.Globbing.Token;

namespace DotNet.Globbing.Generation
{
    internal class WildcardDirectoryTokenMatchGenerator : IMatchGenerator
    {
        private WildcardDirectoryToken token;
        private PathSeparatorKind _PathSeparatorKind = PathSeparatorKind.ForwardSlash;
        private CompositeTokenMatchGenerator _subGenerator;
        private readonly Random _random;

        private int _maxLiteralLength = 10;
        private int _maxSegments = 5;
        private int _minSegments = 0;

        public WildcardDirectoryTokenMatchGenerator(WildcardDirectoryToken token, Random random, CompositeTokenMatchGenerator subGenerator)
        {
            this.token = token;
            _subGenerator = subGenerator;
            _random = random;
        }

        public void AppendMatch(StringBuilder builder)
        {
            AppendRandomPaths(builder);

            // append sub geenrator match.
            _subGenerator.AppendMatch(builder);
        }

        private bool AppendRandomPaths(StringBuilder builder)
        {
            // append a random number of random literals, between 0 characters and 10 in length,
            // seperated by path separators.
            var numberOfSegments = _random.Next(_minSegments, _maxSegments);

            if (token.LeadingPathSeparator != null)
            {
                builder.Append(token.LeadingPathSeparator.Value);
            }
            if (numberOfSegments > 1)
            {
                for (int i = 1; i <= (numberOfSegments - 1); i++)
                {
                    builder.AppendRandomLiteralString(_random, _maxLiteralLength);
                    if (_PathSeparatorKind == PathSeparatorKind.ForwardSlash)
                    {
                        builder.Append('/');
                    }
                    else
                    {
                        builder.Append('\\');
                    }
                }
            }

            builder.AppendRandomLiteralString(_random, _maxLiteralLength);

            if (token.TrailingPathSeparator != null)
            {
                builder.Append(token.TrailingPathSeparator.Value);
            }
            return true;
        }

        public void AppendNonMatch(StringBuilder builder)
        {
            AppendRandomPaths(builder);
            // the only way we wont be able to match is if our subgenerators don't match.
            _subGenerator.AppendNonMatch(builder);
        }

    }
}