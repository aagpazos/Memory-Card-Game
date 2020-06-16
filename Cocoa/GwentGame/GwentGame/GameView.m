//
//  GameView.m
//  GwentGame
//
//  Created by Adrian on 18/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import "GameView.h"
#import "Card.h"

@implementation GameView

@synthesize backgroundName;

- (id)initWithFrame:(NSRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        srandom((unsigned int)time(NULL));
        backgroundName = @"backgroundStart";
    }
    return self;
}

-(void)drawRect:(NSRect)dirtyRect
{
    [super drawRect:dirtyRect];
    NSRect bounds = [self bounds];
    NSGraphicsContext *ctx = [NSGraphicsContext currentContext];
    NSImage *fondo = [NSImage imageNamed:backgroundName];
    [fondo drawInRect:bounds];
    [controlador dibujarBaraja:bounds withGraphicsContext:ctx];
}

-(void)mouseDown:(NSEvent *)event
{
    
    NSPoint p = [self convertPoint:[event locationInWindow] fromView:nil];
    NSRect bounds = [self bounds];
    NSGraphicsContext *ctx = [NSGraphicsContext currentContext];
    [controlador mouseEvent:p withBounds:bounds withGraphicsContext:ctx];
}

@end





