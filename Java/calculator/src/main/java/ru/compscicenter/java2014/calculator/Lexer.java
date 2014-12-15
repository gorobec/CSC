package ru.compscicenter.java2014.calculator;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Created by анастасия on 03.10.2014.
 */

enum LexemType{
    NUMBER,
    PLUS,
    MINUS,
    MULT,
    DIV,
    POW,
    SIN,
    COS,
    ABS,
    OPEN,
    CLOSE,
    OTHER
}

public class Lexer {

    private Pattern doubleRegExp = Pattern.compile("[-+]?[0-9]*\\.?[0-9]+([eE][-+]?[0-9]+)?");
    private Matcher matcher;

    class Lexem
    {
        private double value;
        private LexemType type;

        Lexem(LexemType t)
        {
            type = t;
        }

        LexemType getType ()
        {
            return type;
        }

        void setValue (double x)
        {
            value = x;
        }

        double getValue ()
        {
            if(type == LexemType.NUMBER)
                return value;
            else
                return 0;
        }
    }

    public Lexem currentLexem = new Lexem(LexemType.OTHER);
    private int it = 0;
    private String expression;

    Lexer(String expression)
    {
        this.expression = expression;
        this.matcher = doubleRegExp.matcher(expression);
    }

    void nextLexem()
    {
        if (it >= expression.length())
        {
            this.currentLexem = new Lexem(LexemType.OTHER);
            return;
        }
        switch (expression.charAt(it))
        {
            case ' ':
                ++it;
                nextLexem();
                break;
            case '+':
                ++it;
                currentLexem = new Lexem(LexemType.PLUS);
                break;
            case '-':
                ++it;
                currentLexem = new Lexem(LexemType.MINUS);
                break;
            case '*':
                ++it;
                currentLexem = new Lexem(LexemType.MULT);
                break;
            case '/':
                ++it;
                currentLexem = new Lexem(LexemType.DIV);
                break;
            case '^':
                ++it;
                currentLexem = new Lexem(LexemType.POW);
                break;
            case 's':
                it += 3;
                currentLexem = new Lexem(LexemType.SIN);
                break;
            case 'c':
                it += 3;
                currentLexem = new Lexem(LexemType.COS);
                break;
            case 'a':
                it += 3;
                currentLexem = new Lexem(LexemType.ABS);
                break;
            case '(':
                ++it;
                currentLexem = new Lexem(LexemType.OPEN);
                break;
            case ')':
                ++it;
                currentLexem = new Lexem(LexemType.CLOSE);
                break;
            default:
                matcher.find();
                this.currentLexem = new Lexem(LexemType.NUMBER);
                currentLexem.setValue(Math.abs(Double.parseDouble(matcher.group())));
                it = matcher.end();
        }

    }
}


