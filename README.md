# TestExercise

So, I think I have to clear tome things:
1) I wrote two classes for ex.1 and for ex.2, because in ex.2 i decided to change something to adapt it for concurrency execution.

2) Usually in tests we have cases for success and fail ways. So we should to imagine that sometimes we dont have some accounts for use them in AccountInfo (accounts does not exists), and AccountInfo should have special behavior in this case. It was not in the exercise terms, but without it, i think, tests is not be complete.
That's why I added this code:

 ```
 moq.Setup(x => x.GetAccountAmount(It.IsNotIn(new int[] { 1, 5 })))
                .Throws(new InvalidOperationException());