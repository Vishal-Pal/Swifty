namespace swifty.Code.Text {
    public struct TextSpan {
        public TextSpan(int start, int length) {
            Start = start;
            Length = length;
        }
        public int Start {get;}
        public int Length {get;}
        public int End => Start + Length;

        public static TextSpan FromBound(int start, int end) {
            int length = end - start;
            return new TextSpan(start, length);
        }
    }
}