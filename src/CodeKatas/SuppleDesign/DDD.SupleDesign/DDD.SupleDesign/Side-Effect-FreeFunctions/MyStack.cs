namespace DDD.SuppleDesign.Side_Effect_FreeFunctions;

public class MyStack
{
    private object[] _array;
    private int _size;


    public MyStack()
    {
        _array = new object[10];
        _size = 0;
    }

    public void Push(object obj)
    {
        if (_size == _array.Length)
        {
            var destinationArray = new object[2 * _array.Length];
            Array.Copy((Array)_array, (Array)destinationArray, _size);
            _array = destinationArray;
        }
        _array[_size++] = obj;
    }

    public object Pop()
    {
        if (_size == 0)
            throw new InvalidOperationException();

        var obj = _array[--_size];
        _array[_size] = (object)null;
        return obj;
    }

    public object Peek()
    {
        return _size != 0 
            ? _array[_size - 1] 
            : throw new InvalidOperationException();
    }
}