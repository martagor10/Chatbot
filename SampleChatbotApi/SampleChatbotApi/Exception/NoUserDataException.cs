namespace SampleChatbotApi.Exception;

public class NoUserDataException() : System.Exception("No user data was found in HttpContext");