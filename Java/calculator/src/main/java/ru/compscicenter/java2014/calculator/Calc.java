package ru.compscicenter.java2014.calculator;

/**
 * Created by анастасия on 03.10.2014.
 */
public class Calc implements Calculator
{
    Lexer lexer;

    private double primary()
    {
        double result = 0;
        LexemType curType = lexer.currentLexem.getType();

        switch (curType)
        {
            case OPEN:
                lexer.nextLexem();
                result = expression();
                lexer.nextLexem();
                break;
            case NUMBER:
                result = lexer.currentLexem.getValue();
                lexer.nextLexem();
                break;
            default:
                result = expression();
        }
        return result;
    }

    //Видела пост про полиморфизм, не понимаю, как коротко и красиво заменить свич, где-то же все равно придется это разбирать и будет то же самое
    private double function()
    {
        double result = 0;
        LexemType curType = lexer.currentLexem.getType();

        switch (curType)
        {
            case SIN:
                lexer.nextLexem();
                result = Math.sin(primary());
                break;
            case COS:
                lexer.nextLexem();
                result = Math.cos(primary());
                break;
            case ABS:
                lexer.nextLexem();
                result = Math.abs(primary());
                break;
            default:
                result = primary();
        }
        return result;
    }

    private double power()
    {
        double result = function();
        LexemType curType = lexer.currentLexem.getType();

        if (curType == LexemType.POW)
        {
            lexer.nextLexem();
            result = Math.pow(result, unary());
        }
        return result;
    }

    private double unary()
    {
        double result = 0;
        LexemType curType = lexer.currentLexem.getType();

        switch (curType)
        {
            case MINUS:
                lexer.nextLexem();
                result = -power();
                break;
            case PLUS:
                lexer.nextLexem();
                result = power();
                break;
            default:
                result = power();
        }
        return result;
    }

    private double term()
    {
        double result = unary();
        LexemType curType = lexer.currentLexem.getType();

        while (curType == LexemType.MULT || curType == LexemType.DIV )
        {
            lexer.nextLexem();
            if (curType == LexemType.MULT)
                result *= unary();
            else
                result /= unary();
            curType = lexer.currentLexem.getType();
        }
        return result;
    }

    private double expression()
    {
        double result = term();
        LexemType curType = lexer.currentLexem.getType();

        while (curType == LexemType.PLUS || curType == LexemType.MINUS)
        {
            lexer.nextLexem();
            if (curType == LexemType.PLUS)
                result += term();
            else
                result -= term();
            curType = lexer.currentLexem.getType();
        }
        return result;
    }

    public double calculate(String expression)
    {
        lexer = new Lexer(expression.toLowerCase());
        lexer.nextLexem();
        return expression();
    }
}
