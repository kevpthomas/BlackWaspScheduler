using System;
using System.Linq.Expressions;
using Moq;
using Moq.Language.Flow;

namespace UnitTests
{
    /// <summary>
    /// Extension methods for Moq.
    /// </summary>
    /// <remarks>
    /// Basically allows Moq functionality to be used without needing references to the
    /// <see cref="Moq.Mock" /> that setup a mocked object. This reduces the number of
    /// fields required in tests making them simpler and cleaner.
    /// </remarks>
    public static class MoqExtensions
    {
        /// <summary>
        /// Retrieves the mock that was used to setup the given mocked object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked object.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked object.
        /// </param>
        /// <returns>
        /// The mock.
        /// </returns>
        public static Mock<T> GetMock<T>(this T instance) where T : class
        {
            return Mock.Get(instance);
        }

        /// <summary>
        /// Specifies a setup on the mocked type for a call to a void method.
        /// </summary>
        /// <remarks>
        /// Short cut for
        /// <code>
        /// instance.GetMock().Setup(expression).
        /// </code>
        /// </remarks>
        /// <typeparam name="T">
        /// The type of mocked object.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked object.
        /// </param>
        /// <param name="expression">
        /// Lambda expression that specifies the expected method invocation.
        /// </param>
        /// <returns>
        /// The setup specifier.
        /// </returns>
        public static ISetup<T> Setup<T>(
            this T instance,
            Expression<Action<T>> expression) where T : class
        {
            return instance.GetMock().Setup(expression);
        }

        /// <summary>
        /// Specifies a setup on the mocked type for a call to to a value returning method.
        /// </summary>
        /// <remarks>
        /// Short cut for
        /// <code>
        /// instance.GetMock().Setup(expression).
        /// </code>
        /// </remarks>
        /// <typeparam name="T">
        /// The type of mocked object.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of result from the mocked method.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked object.
        /// </param>
        /// <param name="expression">
        /// Lambda expression that specifies the expected method invocation.
        /// </param>
        /// <returns>
        /// The setup specifier.
        /// </returns>
        public static ISetup<T, TResult> Setup<T, TResult>(
            this T instance,
            Expression<Func<T, TResult>> expression) where T : class
        {
            return instance.GetMock().Setup(expression);
        }

        /// <summary>
        /// Specifies a setup on the mocked type for a call to to a property getter.
        /// </summary>
        /// <typeparam name="T">The type of mocked object.</typeparam>
        /// <typeparam name="TProperty">Type of the property. Typically omitted as it can be inferred from the expression.</typeparam>
        /// <param name="instance">The mocked object.</param>
        /// <param name="expression">Lambda expression that specifies the property getter.</param>
        /// <returns>The setup getter specifier.</returns>
        /// <remarks>If more than one setup is set for the same property getter, the latest one wins and is the one that will be executed.</remarks>
        public static ISetupGetter<T, TProperty> SetupGet<T, TProperty>(
            this T instance,
            Expression<Func<T, TProperty>> expression
            ) where T : class
        {
            return instance.GetMock().SetupGet(expression);
        }

        /// <summary>
        /// Specifies a setup on the mocked type for a call to to a property setter.
        /// </summary>
        /// <typeparam name="T">The type of mocked object.</typeparam>
        /// <param name="instance">The mocked object.</param>
        /// <param name="expression">Lambda expression that sets a property to a value.</param>
        /// <returns>The setup specifier.</returns>
        /// <remarks>If more than one setup is set for the same property setter, the latest one wins and is the one that will be executed.</remarks>
        public static ISetup<T> SetupSet<T>(
            this T instance,
            Action<T> expression
            ) where T : class
        {
            return instance.GetMock().SetupSet(expression);
        }

        /// <summary>
        /// Specifies a setup on the mocked type for a call to to a property setter.
        /// </summary>
        /// <typeparam name="T">The type of mocked object.</typeparam>
        /// <typeparam name="TProperty">Type of the property. Typically omitted as it can be inferred from the expression.</typeparam>
        /// <param name="instance">The mocked object.</param>
        /// <param name="expression">Lambda expression that sets a property to a value.</param>
        /// <returns>The setup setter specifier.</returns>
        /// <remarks>
        /// If more than one setup is set for the same property setter, the latest one wins and is the one that will be executed.
        /// This overloads allows the use of a callback already typed for the property type.
        /// </remarks>
        public static ISetupSetter<T, TProperty> SetupSet<T, TProperty>(
            this T instance,
            Action<T> expression
            ) where T : class
        {
            return instance.GetMock().SetupSet<TProperty>(expression);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock, 
        /// without specifying a failure error message or the number of times a method is allowed to be called. 
        /// Use in conjunction with the default Moq.MockBehaviour.Loose.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of result.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        public static void Verify<T, TResult>(
            this T instance,
            Expression<Func<T, TResult>> expression) where T : class
        {
            instance.Verify(
                expression,
                Times.AtLeastOnce());
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock,
        /// specifying the number of times a method is allowed to be called. 
        /// Use in conjunction with the default Moq.MockBehaviour.Loose.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of result.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="times">
        /// The number of times a method is allowed to be called.
        /// </param>
        public static void Verify<T, TResult>(
            this T instance,
            Expression<Func<T, TResult>> expression,
            Times times) where T : class
        {
            instance.GetMock().Verify(
                expression,
                times);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock, 
        /// specifying a failure error message. 
        /// Use in conjunction with the default Moq.MockBehaviour.Loose.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of result.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="failMessage">
        /// Message to show if verification fails.
        /// </param>
        /// <param name="failMessageFormatArgs">
        /// Format args for the failure message.
        /// </param>
        public static void Verify<T, TResult>(
            this T instance,
            Expression<Func<T, TResult>> expression,
            string failMessage,
            params object[] failMessageFormatArgs) where T : class
        {
            instance.Verify(
                expression,
                Times.AtLeastOnce(),
                failMessage,
                failMessageFormatArgs);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock, 
        /// specifying a failure error and the number of times a method is allowed to be called. 
        /// Use in conjunction with the default Moq.MockBehaviour.Loose.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of result.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="times">
        /// The number of times a method is allowed to be called.
        /// </param>
        /// <param name="failMessage">
        /// Message to show if verification fails.
        /// </param>
        /// <param name="failMessageFormatArgs">
        /// Format args for the failure message.
        /// </param>
        public static void Verify<T, TResult>(
            this T instance,
            Expression<Func<T, TResult>> expression,
            Times times,
            string failMessage,
            params object[] failMessageFormatArgs) where T : class
        {
            instance.GetMock().Verify(
                expression,
                times,
                failMessage.ToFormattedFailMessage(failMessageFormatArgs));
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock, 
        /// without specifying a failure error message or the number of times a method is allowed to be called. 
        /// Use in conjunction with the default Moq.MockBehaviour.Loose.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        public static void Verify<T>(
            this T instance,
            Expression<Action<T>> expression) where T : class
        {
            instance.Verify(
                expression,
                Times.AtLeastOnce());
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock, 
        /// specifying the number of times a method is allowed to be called. 
        /// Use in conjunction with the default Moq.MockBehaviour.Loose.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="times">
        /// The number of times a method is allowed to be called.
        /// </param>
        public static void Verify<T>(
            this T instance,
            Expression<Action<T>> expression,
            Times times) where T : class
        {
            instance.GetMock().Verify(
                expression,
                times);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock, 
        /// specifying a failure error message. 
        /// Use in conjunction with the default Moq.MockBehaviour.Loose.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="failMessage">
        /// Message to show if verification fails.
        /// </param>
        /// <param name="failMessageFormatArgs">
        /// Format args for the failure message.
        /// </param>
        public static void Verify<T>(
            this T instance,
            Expression<Action<T>> expression,
            string failMessage,
            params object[] failMessageFormatArgs) where T : class
        {
            instance.Verify(
                expression,
                Times.AtLeastOnce(),
                failMessage,
                failMessageFormatArgs);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock, 
        /// specifying a failure error message and the number of times a method is allowed to be called. 
        /// Use in conjunction with the default Moq.MockBehaviour.Loose.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="times">
        /// The number of times a method is allowed to be called.
        /// </param>
        /// <param name="failMessage">
        /// Message to show if verification fails.
        /// </param>
        /// <param name="failMessageFormatArgs">
        /// Format args for the failure message.
        /// </param>
        public static void Verify<T>(
            this T instance,
            Expression<Action<T>> expression,
            Times times,
            string failMessage,
            params object[] failMessageFormatArgs) where T : class
        {
            instance.GetMock().Verify(
                expression,
                times,
                failMessage.ToFormattedFailMessage(failMessageFormatArgs));
        }


        /// <summary>
        /// Verifies that a property was retrieved from the mock, 
        /// without specifying a failure error message or the number of times a getter is allowed to be called.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// Type of the property. Typically omitted as it can be inferred from the expression.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        public static void VerifyGet<T, TProperty>(
            this T instance,
            Expression<Func<T, TProperty>> expression) where T : class
        {
            instance.VerifyGet(
                expression,
                Times.AtLeastOnce());
        }

        /// <summary>
        /// Verifies that a property was retrieved from the mock, 
        /// specifying the number of times a getter is allowed to be called.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// Type of the property. Typically omitted as it can be inferred from the expression.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="times">
        /// The number of times a method is allowed to be called.
        /// </param>
        public static void VerifyGet<T, TProperty>(
            this T instance,
            Expression<Func<T, TProperty>> expression,
            Times times) where T : class
        {
            instance.GetMock().VerifyGet(
                expression,
                times);
        }

        /// <summary>
        /// Verifies that a property was retrieved from the mock, 
        /// specifying a failure message.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// Type of the property. Typically omitted as it can be inferred from the expression.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="failMessage">
        /// Message to show if verification fails.
        /// </param>
        public static void VerifyGet<T, TProperty>(
            this T instance,
            Expression<Func<T, TProperty>> expression,
            string failMessage) where T : class
        {
            instance.VerifyGet(
                expression,
                Times.AtLeastOnce(),
                failMessage);
        }

        /// <summary>
        /// Verifies that a property was retrieved from the mock, 
        /// specifying a failure message and the number of times a getter is allowed to be called.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// Type of the property. Typically omitted as it can be inferred from the expression.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="times">
        /// The number of times a method is allowed to be called.
        /// </param>
        /// <param name="failMessage">
        /// Message to show if verification fails.
        /// </param>
        public static void VerifyGet<T, TProperty>(
            this T instance,
            Expression<Func<T, TProperty>> expression,
            Times times,
            string failMessage) where T : class
        {
            instance.GetMock().VerifyGet(
                expression,
                times,
                failMessage);
        }


        /// <summary>
        /// Verifies that a property was set on the mock,
        /// without specifying a failure error message or the number of times a setter is allowed to be called.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        public static void VerifySet<T>(
            this T instance,
            Action<T> expression) where T : class
        {
            instance.VerifySet(
                expression,
                Times.AtLeastOnce());
        }

        /// <summary>
        /// Verifies that a property was set on the mock,
        /// specifying the number of times a setter is allowed to be called.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="times">
        /// The number of times a method is allowed to be called.
        /// </param>
        public static void VerifySet<T>(
            this T instance,
            Action<T> expression,
            Times times) where T : class
        {
            instance.GetMock().VerifySet(
                expression,
                times);
        }

        /// <summary>
        /// Verifies that a property was set on the mock, 
        /// specifying a failure message.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="failMessage">
        /// Message to show if verification fails.
        /// </param>
        public static void VerifySet<T>(
            this T instance,
            Action<T> expression,
            string failMessage) where T : class
        {
            instance.VerifySet(
                expression,
                Times.AtLeastOnce(),
                failMessage);
        }

        /// <summary>
        /// Verifies that a property was set on the mock, 
        /// specifying a failure message and the number of times a setter is allowed to be called.
        /// </summary>
        /// <typeparam name="T">
        /// The type of mocked instance.
        /// </typeparam>
        /// <param name="instance">
        /// The mocked instance.
        /// </param>
        /// <param name="expression">
        /// Expression to verify.
        /// </param>
        /// <param name="times">
        /// The number of times a method is allowed to be called.
        /// </param>
        /// <param name="failMessage">
        /// Message to show if verification fails.
        /// </param>
        public static void VerifySet<T>(
            this T instance,
            Action<T> expression,
            Times times,
            string failMessage) where T : class
        {
            instance.GetMock().VerifySet(
                expression,
                times,
                failMessage);
        }


        /// <summary>
        /// Formats a fail message using the provided formatting arguments.
        /// </summary>
        /// <param name="failMessage">Original fail message.</param>
        /// <param name="failMessageFormatArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>A formatted fail message if format args are present; else the original fail message.</returns>
        private static string ToFormattedFailMessage(this string failMessage, object[] failMessageFormatArgs)
        {
            if (failMessageFormatArgs != null &&
               failMessageFormatArgs.Length > 0)
            {
                return string.Format(
                    failMessage,
                    failMessageFormatArgs);
            }
            return failMessage;
        }
    }
}