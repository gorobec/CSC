package ru.compscicenter.java2014.implementor;

import java.io.File;
import java.lang.reflect.*;
import java.net.URL;
import java.net.URLClassLoader;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.HashSet;
import java.util.Set;

/**
 * Created by анастасия on 22.11.2014.
 */
public class SimpleImplementor implements Implementor {
    private final String outputDirectory;
    private Class toImplement;

    public SimpleImplementor(final String outputDirectory) {
        this.outputDirectory = outputDirectory;
    }

    @Override
    public String implementFromDirectory(final String directoryPath, final String className) throws ImplementorException {
        try {
            File inputDirectory = new File(directoryPath);
            ClassLoader classLoader = new URLClassLoader(new URL[] {inputDirectory.toURI().toURL()});
            toImplement = classLoader.loadClass(className);
            return generateClass();
        } catch (Exception e) {
            throw new ImplementorException("Input class not found");
        }
    }

    @Override
    public String implementFromStandardLibrary(final String className) throws ImplementorException {
        try {
            ClassLoader classLoader = SimpleImplementor.class.getClassLoader();
            toImplement = classLoader.loadClass(className);
            return generateClass();
        } catch (Exception e) {
            throw new ImplementorException("Input class not found");
        }
    }

    private String generateClass() throws ImplementorException {
        try {
            String result = "";
            result += "public class " + toImplement.getSimpleName() + "Impl"
                    + (toImplement.isInterface() ? " implements " : " extends ") + toImplement.getName() + " { ";
            Method[] methods = toImplement.getDeclaredMethods();
            Field[] fields = toImplement.getDeclaredFields();
            Constructor[] constructors = toImplement.getDeclaredConstructors();
            int modifier = toImplement.getModifiers();

            if (Modifier.isFinal(modifier)) {
                throw new ImplementorException("Class is final");
            }

            for (Field field : fields) {
                result += generateField(field);
            }
            for (Constructor constructor : constructors) {
                result += generateConstructor(constructor, toImplement.getSimpleName());
            }

            Set<String> allMethods = new HashSet<>();

            for (Class source = toImplement; source != null; source = source.getSuperclass()) {
                for (Method method : source.getDeclaredMethods()) {
                    if (Modifier.isAbstract(method.getModifiers()) && !(Modifier.isFinal(method.getModifiers()))) {
                        allMethods.add(generateMethod(method));
                    }
                }
                for (Class sourceInterface : source.getInterfaces()) {
                    for (Method method : sourceInterface.getDeclaredMethods()) {
                       if (!(Modifier.isFinal(method.getModifiers()))) {
                           allMethods.add(generateMethod(method));
                       }
                    }
                }
            }
            for (Method method : methods) {
                if (!(Modifier.isFinal(method.getModifiers()))) {
                    allMethods.add(generateMethod(method));
                }
            }

            for (String method : allMethods) {
                result += method;
            }

            result += "}";
            String outputClassName = toImplement.getSimpleName() + "Impl";
            Path newFilePath = Paths.get(outputDirectory, outputClassName + ".java");

            try {
                newFilePath = Files.createFile(newFilePath);
                Files.write(newFilePath, result.getBytes());
            } catch (Exception e) {
                throw new ImplementorException(e.getMessage());
            }

            return outputClassName;
        } catch (Exception e) {
            throw new ImplementorException(e.getMessage());
        }
    }

    private String generateMethod(final Method method) {
        Parameter[] parameters = method.getParameters();
        Class returnType = method.getReturnType();
        String result = Modifier.toString(method.getModifiers()).replace("abstract", "")
                + " " + returnType.getCanonicalName() + " " + method.getName() + "(";

        for (Parameter param : parameters) {
            result += param.getType().getCanonicalName() + " " + param.getName() + ",";
        }
        if (parameters.length > 0) {
            result = result.substring(0, result.length() - 1);
        }
        result += ") { ";

        if (!returnType.isPrimitive()) {
            result += "return null; ";
        } else if (returnType.getCanonicalName().equals("boolean")) {
            result += "return true; ";
        } else if (returnType.getCanonicalName().equals("void")) {
            result += "return; ";
        } else if (returnType.getCanonicalName().equals("double") || returnType.getCanonicalName().equals("false")) {
            result += "return 0.0; ";
        } else {
            result += "return 0; ";
        }
        result += " } \n";
        return result;
    }

    private String generateField(final Field field) {
        String result = Modifier.toString(field.getModifiers()).replace("abstract", "")
                + " " + field.getType().getCanonicalName() + " " + field.getName() + "; ";
        return result;
    }

    private String generateConstructor(final Constructor constructor, final String name) {
        String result = "";
        String modifier = Modifier.toString(constructor.getModifiers()).replace("abstract", "");
        Parameter[] parameters = constructor.getParameters();
        result += modifier + " " + name + "Impl" + "(";

        for (Parameter param : parameters) {
            result += param.getType().getCanonicalName() + " " + param.getName() + ",";
        }
        if (parameters.length > 0) {
            result = result.substring(0, result.length() - 1);
        }
        result += ") {}\n";
        return result;
    }
}
