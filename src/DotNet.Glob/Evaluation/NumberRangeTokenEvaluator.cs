using DotNet.Globbing.Token;
using System;

namespace DotNet.Globbing.Evaluation
{
    public class NumberRangeTokenEvaluator : IGlobTokenEvaluator
    {
        private readonly NumberRangeToken _token;

        public NumberRangeTokenEvaluator(NumberRangeToken token)
        {
            _token = token;
        }
#if NETCOREAPP2_1
        public bool IsMatch(string allChars, int currentPosition, out int newPosition) => IsMatch(allChars.AsSpan(), currentPosition, out newPosition);

        public bool IsMatch(ReadOnlySpan<char> allChars, int currentPosition, out int newPosition)
#else
        public bool IsMatch(string allChars, int currentPosition, out int newPosition)
#endif
        {
            var currentChar = allChars[currentPosition];
            newPosition = currentPosition + 1;

            if (currentChar >= _token.Start && currentChar <= _token.End)
            {
                if (_token.IsNegated)
                {
                    return false; // failed to match
                }
            }
            else
            {
                if (!_token.IsNegated)
                {
                    return false; // failed to match
                }
            }

            return true;
        }

        public virtual int ConsumesMinLength
        {
            get { return 1; }
        }

        public bool ConsumesVariableLength
        {
            get { return false; }
        }
    }
}