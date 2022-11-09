namespace script.Buffer
{
    public class BufferUnit
    {
        public string m_name;
        public string m_intro;

        public BufferUnit(string name, string intro, BufferType type, decimal operand)
        {
            this.m_name = name;
            this.m_intro = intro;
            this.m_type = type;
            this.m_operand = operand;
        }

        public enum BufferType
        {
            Add,Mul,Minus,Div
        }
        public BufferType m_type;
        public decimal m_operand;

        public  decimal Cal(decimal val)
        {
          
            switch (m_type)
            {
                case BufferType.Add:
                    val += m_operand;
                    break;
                case BufferType.Mul:
                    val *= m_operand;
                    break;
                case BufferType.Minus:
                case BufferType.Div:
                default:
                    break;
                    
            }
            return val;
        }

        public override string ToString()
        {
            return $"{nameof(m_name)}: {m_name}, {nameof(m_intro)}: {m_intro}, {nameof(m_type)}: {m_type}, {nameof(m_operand)}: {m_operand}";
        }
    }
}