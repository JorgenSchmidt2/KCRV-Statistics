namespace MathServiceTests.EstimatorsTests.TestEntites
{
    public class CheckData<T>
    {
        public bool IsCorrect { get; set; }
        public T TestValue { get; set; }
        public T ActuallyValue { get; set; }
    }
}