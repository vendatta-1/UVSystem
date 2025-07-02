using System.Diagnostics.CodeAnalysis;

namespace UVS.Domain.Common;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);
}


public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<TValue> ValidationFailure(Error error) =>
        new(default, false, error);
}
public class PagedResult<T> : Result
{
    public IEnumerable<T> Items { get; }

    public int TotalCount { get; }

    public int PageSize { get; }

    public int CurrentPage { get; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    private PagedResult(
        IEnumerable<T> items,
        int totalCount,
        int pageSize,
        int currentPage,
        bool isSuccess,
        Error error)
        : base(isSuccess, error)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    public static PagedResult<T> Success(T[] items, int totalCount, int pageSize, int currentPage)
    {
        return new PagedResult<T>(items, totalCount, pageSize, currentPage, true, Error.None);
    }

    public new static PagedResult<T> Failure(Error error)
    {
        return new PagedResult<T>(new List<T>(), 0, 0, 0, false, error);
    }
}
