namespace APBD6;

public class EntityNotFoundException : Exception {
    public EntityNotFoundException(string message) : base(message) { }
}

public class OrderNotFoundException : Exception {
    public OrderNotFoundException(string message) : base(message) { }
}

public class ConflictException : Exception {
    public ConflictException(string message) : base(message) { }
}