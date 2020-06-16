//
//  Baraja.m
//  GwentGame
//
//  Created by Adrian on 27/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import "Baraja.h"

@implementation Baraja
@synthesize cartas;

-(id)init{
    self=[super init];
    if(!self){
        return nil;
    }
    [self setCartas:[[NSMutableArray alloc]init]];
    return self;
}

-(void)darVueltaCarta:(Card *)card
{
    [card darVueltaCarta];
}

-(int)numCartasEnLaBaraja
{
    return (int)cartas.count;
}

-(void)addCard:(Card *)card
{
    [cartas addObject:card];
}

-(void)deleteCard:(Card *)card
{
    [cartas removeObject:card];
}

-(Card *)cardWithIndex:(NSInteger)i
{
    return [cartas objectAtIndex:i];
}

@end
